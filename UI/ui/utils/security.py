import base64
import datetime
import os
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.primitives import padding
from cryptography.hazmat.backends import default_backend
from django.contrib import messages
from django.shortcuts import render, redirect
from ui.utils.api import call_api, handle_API_error

# 256-bit key (must match your C# key, keep this secure!)
AES_SECRET_KEY = b'32-byte-long-secret-key-here-12!'  # 32 bytes exactly

def encrypt_aes(data):
    # Pad the data to a multiple of 16 bytes (AES block size)
    padder = padding.PKCS7(128).padder()
    padded_data = padder.update(data.encode()) + padder.finalize()

    # Generate a random IV
    iv = os.urandom(16)

    cipher = Cipher(algorithms.AES(AES_SECRET_KEY), modes.CBC(iv), backend=default_backend())
    encryptor = cipher.encryptor()
    encrypted = encryptor.update(padded_data) + encryptor.finalize()

    return {
        'ciphertext': base64.b64encode(encrypted).decode(),
        'iv': base64.b64encode(iv).decode()
    }

def get_client_ip(request):
    x_forwarded_for = request.META.get('HTTP_X_FORWARDED_FOR')
    return x_forwarded_for.split(',')[0] if x_forwarded_for else request.META.get('REMOTE_ADDR')

def get_metadata(request):
    return {
            'browser': request.META.get('HTTP_USER_AGENT', ''),
            'ip_address': get_client_ip(request),
            'accept_language': request.META.get('HTTP_ACCEPT_LANGUAGE', ''),
            'referer': request.META.get('HTTP_REFERER', ''),
            'host': request.META.get('HTTP_HOST', ''),
            'request_method': request.method,
            'query_string': request.META.get('QUERY_STRING', ''),
            'session_key': request.session.session_key,
            'cookies': dict(request.COOKIES),
        }

def login_view(request, process, redirect_path, login_path):
    context = {}
    context['hide_nav'] = True
    request.session['last_activity'] = datetime.datetime.utcnow().isoformat()

    if 'inactivity_message' in request.session:
        messages.warning(request, request.session['inactivity_message'])
        del request.session['inactivity_message']

    if request.method == 'POST':
        username = request.POST['username']
        password = request.POST['password']

        # Encrypt sensitive fields
        encrypted_username = encrypt_aes(username)
        encrypted_password = encrypt_aes(password)

        payload = {
            'Process': process,
            'Username': encrypted_username['ciphertext'],
            'Password': encrypted_password['ciphertext'],
            'iv_username': encrypted_username['iv'],
            'iv_password': encrypted_password['iv'],
            'client_metadata': get_metadata(request)
        }

        try:
            result = call_api(request, payload)
            request.session['last_activity'] = datetime.datetime.utcnow().isoformat()

            if result.json().get('authenticated'):
                response = redirect(redirect_path)
                response.set_cookie('securityToken', result.json().get('securityToken'))
                return response
            else:
                context['error'] = result.json().get('errorMessage')
                return render(request, login_path, context)

        except Exception as e:
            handle_API_error(request, login_path, context)

    return render(request, login_path, context)
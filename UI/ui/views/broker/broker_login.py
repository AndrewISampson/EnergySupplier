import json
import requests
from django.shortcuts import render, redirect
from django.contrib.auth import login
from django.http import HttpResponse
from django.template import loader
from ui.views.generic.security import *

def home(request):
    template = loader.get_template('broker/broker_login.html')
    return HttpResponse(template.render())

def broker_login_view(request):
    if request.method == 'POST':
        """ username = request.POST['username']
        password = request.POST['password']
        remember = request.POST.get('remember')

        # Encrypt sensitive fields
        encrypted_username = encrypt_aes(username, AES_SECRET_KEY)
        encrypted_password = encrypt_aes(password, AES_SECRET_KEY)

        payload = {
            'username': encrypted_username['ciphertext'],
            'password': encrypted_password['ciphertext'],
            'iv_username': encrypted_username['iv'],
            'iv_password': encrypted_password['iv'],
            'client_metadata': get_metadata(request)
        } """

        username = request.POST.get('username')
        password = request.POST.get('password')
        remember = request.POST.get('remember') == 'on'

        # Optional: collect browser metadata (if passed via JS or headers)
        user_agent = request.META.get('HTTP_USER_AGENT')
        ip_address = request.META.get('REMOTE_ADDR')

        payload = {
            'username': username,
            'password': password,
            'remember': remember,
            'user_agent': user_agent,
            'ip_address': ip_address,
        }

        try:
            response = requests.post(
                'https://localhost:7001/api/websiterequest',
                headers={'Content-Type': 'application/json'},
                data=json.dumps(payload)
            )
            response.raise_for_status()
            result = response.json()

            if result.get('authenticated'):
                # login logic if needed
                return redirect('/broker/dashboard/')
            else:
                return render(request, 'broker/broker_login.html', {
                    'error': 'Invalid credentials'
                })

        except requests.RequestException as e:
            return render(request, 'broker/broker_login.html', {
                'error': 'Login failed: API error'
            })

    return render(request, 'broker/broker_login.html')

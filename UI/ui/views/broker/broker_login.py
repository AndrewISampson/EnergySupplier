from django.shortcuts import render, redirect
from ui.views.generic.security import *
from ui.utils.api import call_api

def broker_login_view(request):
    context = {}
    context['hide_nav'] = True

    if request.method == 'POST':
        username = request.POST['username']
        password = request.POST['password']
        remember = request.POST.get('remember')

        # Encrypt sensitive fields
        encrypted_username = encrypt_aes(username, AES_SECRET_KEY)
        encrypted_password = encrypt_aes(password, AES_SECRET_KEY)

        payload = {
            'Process': '17902a34-9755-4c64-97c9-5deffc2eeba2',
            'Username': encrypted_username['ciphertext'],
            'Password': encrypted_password['ciphertext'],
            'iv_username': encrypted_username['iv'],
            'iv_password': encrypted_password['iv'],
            'client_metadata': get_metadata(request)
        }

        try:
            result = call_api(payload)

            if result.json().get('authenticated'):
                return redirect('broker_dashboard')
            else:
                context['error'] = 'Invalid credentials'
                return render(request, 'broker/broker_login.html', context)

        except Exception as e:
            context['error'] = 'Login failed: API error'
            return render(request, 'broker/broker_login.html', context)

    return render(request, 'broker/broker_login.html', context)

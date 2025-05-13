import requests
from django.shortcuts import render, redirect
from ui.views.generic.security import *

def broker_login_view(request):
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
            api_url = 'https://localhost:7001/api/websiterequest'
            response = requests.post(
                api_url,
                json=payload,
                verify=False)

            response.raise_for_status()
            result = response.json()

            if result.get('authenticated'):
                return redirect('broker_dashboard')
            else:
                return render(request, 'broker/broker_login.html', {
                    'error': 'Invalid credentials'
                })

        except Exception as e:
            print(str(e))
            return render(request, 'broker/broker_login.html', {
                'error': 'Login failed: API error'
            })

    return render(request, 'broker/broker_login.html')

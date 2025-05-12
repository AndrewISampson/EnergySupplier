""" import requests
from django.http import HttpResponse
from django.template import loader

def home(request):
    # Data to send to the API
    payload = {"Process": "17902a34-9755-4c64-97c9-5deffc2eeba2"}

    try:
        response = requests.post(
            "https://localhost:7001/api/websiterequest",  # Your API URL
            json=payload,
            verify=False  # Only for self-signed certs during development
        )
        response.raise_for_status()
        api_data = response.json()  # Parse JSON response from API
    except requests.exceptions.RequestException as e:
        api_data = {'error': f'API call failed: {str(e)}'}

    # Load and render the template
    template = loader.get_template('broker/broker_login.html')
    return HttpResponse(template.render({'api_data': api_data}, request)) """

import requests
from django.shortcuts import render, redirect
from django.contrib import messages
from django.contrib.auth import login
from django.contrib.auth.models import User
from django.http import HttpResponse
from django.template import loader
from ui.views.generic.security import *

def home(request):
    template = loader.get_template('broker/broker_login.html')
    return HttpResponse(template.render())

def broker_login_view(request):
    if request.method == 'POST':
        username = request.POST['username']
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
        }

        try:
            api_url = 'https://your-csharp-api.com/api/authenticate'
            response = requests.post(api_url, json=payload)
            response_data = response.json()

            if response.status_code == 200 and response_data.get('authenticated') is True:
                user, _ = User.objects.get_or_create(username=username)
                login(request, user)
                if not remember:
                    request.session.set_expiry(0)
                return redirect('broker/broker_dashboard.html')
            else:
                messages.error(request, 'Invalid username or password.')
        except requests.RequestException as e:
            messages.error(request, f'Login service error: {str(e)}')

    return render(request, 'broker/broker_login.html')

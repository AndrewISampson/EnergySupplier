import datetime
from django.contrib import messages
from django.shortcuts import render, redirect
from ui.utils.security import encrypt_aes, get_metadata
from ui.utils.api import call_api, handle_API_error

def broker_login_view(request):
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
            'Process': '17902a34-9755-4c64-97c9-5deffc2eeba2',
            'Username': encrypted_username['ciphertext'],
            'Password': encrypted_password['ciphertext'],
            'iv_username': encrypted_username['iv'],
            'iv_password': encrypted_password['iv'],
            'client_metadata': get_metadata(request)
        }

        try:
            result = call_api(payload)
            request.session['last_activity'] = datetime.datetime.utcnow().isoformat()

            if result.json().get('authenticated'):
                return redirect('broker_dashboard')
            else:
                context['error'] = result.json().get('errorMessage')
                return render(request, 'broker/broker_login.html', context)

        except Exception as e:
            handle_API_error(request, 'broker/broker_login.html', context)

    return render(request, 'broker/broker_login.html', context)

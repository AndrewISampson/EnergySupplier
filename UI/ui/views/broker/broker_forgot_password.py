from django.shortcuts import render, redirect
from django.views.decorators.csrf import csrf_exempt
from ui.utils.api import call_api, handle_API_error
from ui.utils.security import encrypt_aes

@csrf_exempt
def home(request):
    URL = 'broker/broker_forgot_password.html'
    step = request.POST.get('step', 'email')
    context = {}

    if request.method == 'POST':
        email = request.POST.get('email')
        encrypted_username = encrypt_aes(email)
        context['email'] = email
        payload = {
                'Process': 'c9156484-fde3-4259-86cd-24201063f265',
                'Username': encrypted_username['ciphertext'],
                'iv_username': encrypted_username['iv'],
                'Step': step
            }

        if step == 'email':
            try:
                result = call_api(request, payload)

                if result.json().get('valid'):
                    context['step'] = 'code'
                    context['message'] = result.json().get('message')
                else:
                    context['step'] = 'email'
                    context['error'] = result.json().get('message')

            except Exception as e:
                handle_API_error(request, URL, context)

        elif step == 'code':
            encrypted_code = encrypt_aes(request.POST.get('code'))
            payload['ValidationCode'] = encrypted_code['ciphertext']
            payload['iv_ValidationCode'] = encrypted_code['iv']

            try:
                result = call_api(request, payload)

                if result.json().get('valid'):
                    context['step'] = 'reset'
                else:
                    context['step'] = 'code'
                    context['error'] = result.json().get('message')

            except Exception as e:
                handle_API_error(request, URL, context)

        elif step == 'reset':
            encrypted_password = encrypt_aes(request.POST.get('new_password'))
            payload['Password'] = encrypted_password['ciphertext']
            payload['iv_password'] = encrypted_password['iv']

            try:
                result = call_api(request, payload)

                if result.json().get('valid'):
                    return redirect('broker_login')
                else:
                    context['step'] = 'reset'
                    context['error'] = result.json().get('message')

            except Exception as e:
                handle_API_error(request, URL, context)
        
        else:
            context['step'] = 'email'

    else:
        context['step'] = 'email'

    context['hide_nav'] = True
    return render(request, URL, context)
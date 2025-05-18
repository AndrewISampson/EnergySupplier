from django.shortcuts import render, redirect
from django.views.decorators.csrf import csrf_exempt
import requests
from ui.utils.api import call_api

@csrf_exempt
def home(request):
    step = request.POST.get('step', 'email')
    context = {}

    if step == 'email' and request.method == 'POST':
        email = request.POST.get('email')

        payload = {
            'Process': 'broker_forgot_password_part_1',
            'Username': email
        }

        response = call_api(payload)

        context['step'] = 'code'
        context['email'] = email
        context['message'] = 'If the email exists, a reset code has been sent.'

    elif step == 'code' and request.method == 'POST':
        email = request.POST.get('email')
        code = request.POST.get('code')

        payload = {
            'Process': 'broker_forgot_password_part_2',
            'Username': email,
            'ValidationCode': code
        }

        response = call_api(payload)

        if response.status_code == 200 and response.json().get('valid'):
            context['step'] = 'reset'
            context['email'] = email
        else:
            context['step'] = 'code'
            context['email'] = email
            context['error'] = 'Invalid code. Please try again.'

    elif step == 'reset' and request.method == 'POST':
        email = request.POST.get('email')
        new_password = request.POST.get('new_password')

        payload = {
            'Process': 'broker_forgot_password_part_3',
            'Username': email,
            'Password': new_password
        }

        response = call_api(payload)

        if response.status_code == 200:
            return redirect('broker_login')
        else:
            context['step'] = 'reset'
            context['email'] = email
            context['error'] = 'Password reset failed. Try again.'

    else:
        context['step'] = 'email'

    context['hide_nav'] = True
    return render(request, 'broker/broker_forgot_password.html', context)
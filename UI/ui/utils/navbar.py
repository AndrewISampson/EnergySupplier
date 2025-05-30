from django.shortcuts import redirect
from ui.utils.api import call_api


def determine_navbar_type(request):
    path = request.path

    payload = {
            'Process': 'd39723b0-3824-4445-88d5-ef7692c13c71',
            'Path': request.path,
            'SecurityToken' : request.COOKIES.get('securityToken')
        }
    
    result = call_api(payload)

    if result.json().get('valid'):
        return result.json().get('navbar')

    else:
        if path.startswith('/broker/'):
            return redirect('broker_login')
        elif path.startswith('/customer/'):
            return redirect('customer_login')
        elif path.startswith('/internal/'):
            return redirect('internal_login')
        else:
            return redirect('master')
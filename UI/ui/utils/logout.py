from django.views.decorators.csrf import csrf_protect
from django.shortcuts import redirect

@csrf_protect
def logout(request, source):

    if request.method == 'POST':
        if source == 'broker':
            response = redirect('broker_login')
        elif source == 'customer':
            response = redirect('customer_login')
        elif source == 'internal':
            response = redirect('internal_login')
        else:
            response = redirect('home')
    else:
        # Optional: redirect or return error on GET
        response = redirect('home')
    
    request.session.flush()
    response.set_cookie('securityToken', '')
    return response
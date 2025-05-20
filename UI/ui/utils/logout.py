from django.views.decorators.csrf import csrf_protect
from django.shortcuts import redirect

@csrf_protect
def logout(request, source):
    if request.method == 'POST':
        if source == 'broker':
            return redirect('broker_login')
        elif source == 'customer':
            return redirect('customer_login')
        elif source == 'internal':
            return redirect('internal_login')
        else:
            return redirect('home')
    else:
        # Optional: redirect or return error on GET
        return redirect('home')

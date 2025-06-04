from ui.utils.security import login_view

def broker_login_view(request):
    return login_view(request, '17902a34-9755-4c64-97c9-5deffc2eeba2', 'broker_dashboard', 'broker/broker_login.html')
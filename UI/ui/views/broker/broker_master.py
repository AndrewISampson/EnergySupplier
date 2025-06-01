from django.http import HttpResponse
from django.shortcuts import redirect, render

from ui.utils.navbar import determine_navbar_type

def home(request):
    return redirect('broker_login')

def load_broker_page(request, page, context=None):
    navbar_type = determine_navbar_type(request)
    
    if isinstance(navbar_type, HttpResponse):
        return navbar_type

    if context is None:
        context = {}

    context["navbar_type"] = navbar_type    
    return render(request, page, context)
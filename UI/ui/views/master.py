from django.http import HttpResponse
from django.shortcuts import render
from ui.utils.navbar import determine_navbar_type

def home(request):
    navbar_type = determine_navbar_type(request)
    
    if isinstance(navbar_type, HttpResponse):
        return navbar_type
    
    return render(request, 'master.html', {
        "navbar_type": navbar_type
    })
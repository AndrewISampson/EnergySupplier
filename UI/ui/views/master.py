from django.shortcuts import render
from ui.utils.navbar import determine_navbar_type

def home(request):
    navbar_type = determine_navbar_type(request)
    return render(request, 'master.html', {
        "navbar_type": navbar_type
    })
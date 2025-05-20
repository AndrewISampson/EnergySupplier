from django.shortcuts import render

def home(request):
    return render(request, 'internal/internal_login.html')
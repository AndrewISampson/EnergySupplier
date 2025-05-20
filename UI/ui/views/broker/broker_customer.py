from django.shortcuts import render

def home(request):
    return render(request, 'broker/broker_customer.html')
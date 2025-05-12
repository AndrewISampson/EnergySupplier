from django.http import HttpResponse
from django.template import loader

def home(request):
    template = loader.get_template('customer/customer_dashboard.html')
    return HttpResponse(template.render())
from django.http import HttpResponse
from django.template import loader

def home(request):
    template = loader.get_template('customer/customer_profile.html')
    return HttpResponse(template.render())
""" import requests
from django.http import HttpResponse
from django.template import loader

def home(request):
    # Data to send to the API
    payload = {"Process": "17902a34-9755-4c64-97c9-5deffc2eeba2"}

    try:
        response = requests.post(
            "https://localhost:7001/api/websiterequest",  # Your API URL
            json=payload,
            verify=False  # Only for self-signed certs during development
        )
        response.raise_for_status()
        api_data = response.json()  # Parse JSON response from API
    except requests.exceptions.RequestException as e:
        api_data = {'error': f'API call failed: {str(e)}'}

    # Load and render the template
    template = loader.get_template('broker/broker_login.html')
    return HttpResponse(template.render({'api_data': api_data}, request)) """
import requests
from django.conf import settings

def call_api(payload, method='POST'):
    """
    Sends a JSON payload to the C# API and returns the response.
    
    Args:
        payload (dict): JSON data to send
        method (str): HTTP method, default is 'POST'
        verify_ssl (bool): Whether to verify SSL cert, default is True
    
    Returns:
        dict: JSON response from API
    """
    url = f"{settings.API_BASE_URL}"
    verify_ssl = not getattr(settings, 'SKIP_API_SSL_VERIFICATION', False)

    try:
        if method == 'POST':
            response = requests.post(url, json=payload, verify=verify_ssl)
        elif method == 'GET':
            response = requests.get(url, params=payload, verify=verify_ssl)
        else:
            raise ValueError(f"Unsupported HTTP method: {method}")

        response.raise_for_status()
        return response
    except requests.RequestException as e:
        return {'error': str(e), 'status': 'api_call_failed'}

from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt
import json
from ui.utils.api import call_api

@csrf_exempt
def update_entity_detail(request):
    if request.method == 'POST':
        data = json.loads(request.body)

        payload = {
            'Process': '8a4bd388-9a05-4e80-aa5a-6f38f71a5fd0',
            'Entity': data['entity'],
            'Id': data['id'],
            'NewDescription': data['new_description'],
            'SecurityToken' : request.COOKIES.get('securityToken')
        }
        result = call_api(request, payload)
        
        if result.json().get('valid'):
            return JsonResponse({'updated_description': data['new_description']})
        
        return JsonResponse({'error': result.json().get('errorMessage')}, status=400)
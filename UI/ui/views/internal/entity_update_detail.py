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
            'EntityId': data['entity_id'],
            'Id': data['id'],
            'NewAttribute': data['new_attribute'],
            'NewDescription': data['new_description']
        }
        result = call_api(request, payload)
        
        if result.json().get('valid'):
            return JsonResponse({'success': True}, status=200)
        
        return JsonResponse({'error': result.json().get('errorMessage')}, status=400)
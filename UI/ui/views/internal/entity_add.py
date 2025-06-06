from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt
import json
from ui.utils.api import call_api

@csrf_exempt
def add_entity(request):
    if request.method == 'POST':
        data = json.loads(request.body)

        payload = {
            'Process': 'facd94b9-fb3e-4094-a87e-29c921db0c91',
            'Entity': data['details'].get('entity'),
            'Attributes': data['details'].get('attributes', []),
            'Descriptions': data['details'].get('descriptions', [])
        }
        result = call_api(request, payload)
        
        if result.json().get('valid'):
            return JsonResponse({'new_entity_id': result.json().get('new_entity_id')}, status=200)
        
        return JsonResponse({'error': result.json().get('errorMessage')}, status=400)
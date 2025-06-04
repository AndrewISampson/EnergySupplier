import json
from django.core.paginator import Paginator
from django.shortcuts import render
from ui.utils.api import call_api
from ui.views.broker.broker_master import load_broker_page

def entity_list_view(request, route):
    app_label, base_model = route.split('.')

    payload = {
        'Process': 'ccb10f91-87fd-47d2-b04d-3566a85445c5',
        'Entity': app_label
    }

    result = call_api(payload)
    raw = result.json().get('entityList')
    records = json.loads(raw) if isinstance(raw, str) else raw

    # Dynamically get the ID field name
    primary_id_field = None
    if records:
        # E.g. 'BrokerId', 'CustomerId'
        for key in records[0].keys():
            if key.lower() == f"{base_model.lower()}id":
                primary_id_field = key
                break

    if not primary_id_field:
        return render(request, 'internal/invalid_entity.html', {
            'route': route,
            'message': f"Could not determine ID field for entity '{base_model}'"
        }, status=400)

    # Paginate
    paginator = Paginator(records, 10)
    page_obj = paginator.get_page(request.GET.get('page'))

    return load_broker_page(request, 'internal/entity_list.html', {
        'route': route,
        'entity_name': base_model,
        'page_obj': page_obj,
        'attribute_field': primary_id_field
    })

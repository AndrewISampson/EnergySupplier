import json
from django.shortcuts import render
from django.http import Http404
from ui.utils.api import call_api
from ui.views.broker.broker_master import load_broker_page

def entity_detail_view(request, route, entity_id):
    app_label, base_model = route.split('.')
    detail_name = request.GET.get('name', '')

    payload = {
        'Process': 'a3ebe6c6-c0cd-4a2d-842b-58fc5b71c7fb',
        'Entity': base_model,
        'EntityId': entity_id
    }

    result = call_api(payload)
    raw = result.json().get('entityList')
    records = json.loads(raw) if isinstance(raw, str) else raw

    detail_dict = {}

    if isinstance(records, list):
        for record in records:
            rec_id = record.get("ID") or record.get("id") or record.get("Id") or "unknown"
            detail_dict[str(rec_id)] = {
                "Attribute": record.get("Attribute", ""),
                "Description": record.get("Description", ""),
                "Action": f'<a href="/edit/{rec_id}" class="btn btn-sm btn-primary">Edit</a>'
            }
    elif isinstance(records, dict):
        detail_dict = {
            str(records.get("ID", "unknown")): {
                "Attribute": records.get("Attribute", ""),
                "Description": records.get("Description", ""),
                "Action": f'<a href="/edit/{records.get("ID")}" class="btn btn-sm btn-primary">Edit</a>'
            }
        }
    else:
        detail_dict = {}

    return load_broker_page(request, 'internal/entity_detail.html', {
        'route': route,
        'entity_name': base_model,
        'detail_name': detail_name,
        'detail': detail_dict
    })

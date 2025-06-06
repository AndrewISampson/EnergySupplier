import json
from ui.utils.api import call_api
from ui.views.broker.broker_master import load_broker_page

def entity_add_view(request, route):
    entity = request.GET.get('entity')

    payload = {
        'Process': 'a3ebe6c6-c0cd-4a2d-842b-58fc5b71c7fb',
        'Entity': entity,
        'EntityId': 0
    }

    result = call_api(request, payload)
    raw = result.json().get('entityList')
    records = json.loads(raw) if isinstance(raw, str) else raw

    detail_dict = {}

    if isinstance(records, list):
        for record in records:
            rec_id = record.get("Id")
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

    return load_broker_page(request, 'internal/entity_add.html', {
        'entity_name': entity,
        'detail_name': 'New ' + entity,
        'detail': detail_dict
    })

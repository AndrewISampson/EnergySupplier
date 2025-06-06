import json
from django.core.paginator import Paginator, EmptyPage, PageNotAnInteger
from ui.utils.api import call_api
from ui.views.broker.broker_master import load_broker_page

def entity_detail_view(request, route, entity_id):
    detail_name = request.GET.get('name', '')
    entity = request.GET.get('entity')

    payload = {
        'Process': 'a3ebe6c6-c0cd-4a2d-842b-58fc5b71c7fb',
        'Entity': entity,
        'EntityId': entity_id
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

    detail_items = list(detail_dict.items())

    paginator = Paginator(detail_items, 10)
    page_number = request.GET.get('page')

    try:
        paginated_details = paginator.page(page_number)
    except PageNotAnInteger:
        paginated_details = paginator.page(1)
    except EmptyPage:
        paginated_details = paginator.page(paginator.num_pages)

    used_attributes = [record.get("Attribute") for record in records if record.get("Attribute")]

    payload = {
        'Process': '1abe91c6-7730-4769-8a6d-d899bda1f639',
        'Entity': entity
    }

    result = call_api(request, payload)
    raw = result.json().get('entityList')
    records = json.loads(raw) if isinstance(raw, str) else raw

    all_attributes = [record.get("Description") for record in records]
    unused_attributes = list(set(all_attributes) - set(used_attributes))
    unused_attributes.sort()
    unused_attributes.insert(0, "")

    return load_broker_page(request, 'internal/entity_detail.html', {
        'entity_name': entity,
        'entity_id': entity_id,
        'detail_name': detail_name,
        'paginated_details': paginated_details,
        'unused_attributes': unused_attributes,
    })

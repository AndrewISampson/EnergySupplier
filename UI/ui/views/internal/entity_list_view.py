import json
from django.core.paginator import Paginator
from ui.utils.api import call_api
from ui.views.broker.broker_master import load_broker_page

def entity_list_view(request, route):
    app_label, base_model = route.split('.')

    payload = {
        'Process': 'ccb10f91-87fd-47d2-b04d-3566a85445c5',
        'Entity': base_model
    }

    result = call_api(request, payload)
    raw = result.json().get('entityList')
    records = json.loads(raw) if isinstance(raw, str) else raw

    data_table_identifier = 'Unknown'
    if records:
        data_table_identifier = records[0].get('DataTableIdentifier')

    paginator = Paginator(records, 10)
    page_obj = paginator.get_page(request.GET.get('page'))

    return load_broker_page(request, 'internal/entity_list.html', {
        'route': route,
        'entity_name': base_model,
        'page_obj': page_obj,
        'data_table_identifier': data_table_identifier
    })

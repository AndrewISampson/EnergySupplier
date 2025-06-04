# ui/views/internal/entity_detail_view.py

from django.shortcuts import render
from django.http import Http404
from ui.utils.api import call_api
from ui.views.broker.broker_master import load_broker_page

def entity_detail_view(request, route, entity_id):
    """
    Fetches all attribute‐value pairs for a given entity ID via the API 
    and renders them in a table.
    
    Expects:
      • route      = "<AppLabel>.<ModelName>" (e.g. "Broker.Broker")
      • entity_id  = primary key of the entity (e.g. BrokerId, CustomerId, etc.)
    
    The API is expected to return JSON like:
      {
        "entityDetail": {
            "BrokerId": 123,
            "Name": "Acme Brokers Ltd",
            "Address": "123 Main St",
            "Phone": "555-1234",
            …
        }
      }
    (Or similarly for Customer, Process, etc.)
    """

    # 1) Split route into app_label and base_model (e.g. "Broker", "Broker")
    try:
        app_label, base_model = route.split('.')
    except ValueError:
        raise Http404(f"Invalid route: '{route}'")

    # 2) Build payload for the API call
    #    You must replace 'YOUR_DETAIL_PROCESS_GUID' with the actual Process GUID your API expects
    payload = {
        'Process': 'YOUR_DETAIL_PROCESS_GUID',
        'Entity': app_label,
        'EntityId': entity_id
    }

    # 3) Call the API
    response = call_api(payload)
    if not response.ok:
        raise Http404(f"API error fetching {base_model} details (ID {entity_id}).")

    data = response.json()
    detail = data.get('entityDetail')
    if not detail:
        raise Http404(f"No details found for {base_model} ID {entity_id}.")

    # 4) Render the template, passing in:
    #    • route         (so "Back to List" can reconstruct the URL)
    #    • entity_name   (e.g. "Broker" or "Customer")
    #    • entity_id     (the numeric ID)
    #    • detail        (a dict of all fields/values)
    return load_broker_page(request, 'internal/entity_detail.html', {
        'route':       route,
        'entity_name': base_model,
        'entity_id':   entity_id,
        'detail':      detail
    })

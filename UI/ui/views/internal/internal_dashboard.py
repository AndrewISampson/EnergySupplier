from ui.views.broker.broker_master import load_broker_page

def home(request):
    return load_broker_page(request, 'internal/internal_dashboard.html')
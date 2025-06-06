from ui.views.broker.broker_master import load_broker_page

def home(request):
    pages = sorted([
        'Broker',
        'User',
        'Process',
        'Setting',
        'Customer'
    ])

    return load_broker_page(request, 'internal/internal_configuration.html', {
        'pages': pages
    })
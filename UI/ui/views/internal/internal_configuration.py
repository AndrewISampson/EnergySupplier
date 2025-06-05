from ui.views.broker.broker_master import load_broker_page

def home(request):
    pages = sorted([
        ('Broker.Broker', 'Broker'),
        ('Administration.User', 'User'),
        ('Internal.Process', 'Process'),
        ('Internal.Setting', 'Setting'),
        ('Customer.Customer', 'Customer')
    ], key=lambda x: x[1])

    return load_broker_page(request, 'internal/internal_configuration.html', {
        'pages': pages
    })
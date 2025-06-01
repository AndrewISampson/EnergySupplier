from datetime import datetime
from dateutil.relativedelta import relativedelta
from ui.views.broker.broker_master import load_broker_page

def home(request):
    today = datetime.today()
    months = [(today + relativedelta(months=i)).strftime("%b %Y") for i in range(12)]
    
    # Replace with actual commission data lookup
    commission_data = [1200, 1450, 1600, 1400, 1700, 1800, 1650, 1550, 1750, 1900, 1850, 2000]
    
    return load_broker_page(request, 'broker/broker_dashboard.html', {
        "months": months,
        "commission_data": commission_data,
    })
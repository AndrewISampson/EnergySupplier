from django.urls import path
from ui.views.broker import broker_master, broker_login, broker_dashboard, broker_commission, broker_customer

urlpatterns = [
    path("", broker_master.home, name="broker_login"),
    path("login", broker_login.broker_login_view, name="broker_login"),
    path("dashboard", broker_dashboard.home, name="broker_dashboard"),
    path("customer", broker_customer.home, name="broker_customer"),
    path("commission", broker_commission.home, name="broker_commission"),
]
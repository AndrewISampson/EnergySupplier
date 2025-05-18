from django.urls import path
from ui.views.broker import broker_master, broker_login, broker_dashboard, broker_commission, broker_customer
from ui.views.broker import broker_forgot_password

urlpatterns = [
    path("", broker_master.home, name="broker_login"),
    path("login", broker_login.broker_login_view, name="broker_login"),
    path("dashboard", broker_dashboard.home, name="broker_dashboard"),
    path("customer", broker_customer.home, name="broker_customer"),
    path("commission", broker_commission.home, name="broker_commission"),
    path("forgot_password", broker_forgot_password.home, name="broker_forgot_password"),
]
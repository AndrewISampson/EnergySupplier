from django.urls import path
from ui.views.internal import internal_master, internal_login, internal_dashboard

urlpatterns = [
    path("", internal_master.home, name="internal_master"),
    path("login", internal_login.home, name="internal_login"),
    path("dashboard", internal_dashboard.home, name="internal_dashboard"),
]
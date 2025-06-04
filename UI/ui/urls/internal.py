from django.urls import path
from ui.views.internal import internal_master, internal_login, internal_dashboard, internal_configuration
from ui.views.internal.entity_list_view import entity_list_view
from ui.views.internal.entity_detail_view import entity_detail_view

urlpatterns = [
    path("", internal_master.home, name="internal_master"),
    path("login", internal_login.home, name="internal_login"),
    path("dashboard", internal_dashboard.home, name="internal_dashboard"),
    path("configuration", internal_configuration.home, name="internal_configuration"),
    path('<str:route>/', entity_list_view, name='entity_list_view'),
    path('<str:route>/<int:entity_id>/', entity_detail_view, name='entity_detail_view'),
]
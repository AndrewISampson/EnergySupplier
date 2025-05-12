from django.urls import path
from ui.views import master

urlpatterns = [
    path("", master.home, name="master"),
]
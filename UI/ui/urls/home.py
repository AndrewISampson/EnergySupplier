from django.urls import path
from ui.views import master
from ui.utils import logout

urlpatterns = [
    path("", master.home, name="master"),
    path('logout/<str:source>/', logout.logout, name='logout'),
]
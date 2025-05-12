from django.urls import include, path

urlpatterns = [
    path("", include("ui.urls.home")),
    path("broker/", include("ui.urls.broker")),
    path("customer/", include("ui.urls.customer")),
    path("internal/", include("ui.urls.internal")),
]
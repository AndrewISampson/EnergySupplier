from django.urls import path
from ui.views.customer import (
    customer_master, customer_login, customer_dashboard,
    customer_eagleeye, customer_contracts, customer_flex,
    customer_bills, customer_profile
)

urlpatterns = [
    path("", customer_master.home, name="customer_master"),
    path("login", customer_login.home, name="customer_login"),
    path("dashboard", customer_dashboard.home, name="customer_dashboard"),
    path("eagleeye", customer_eagleeye.home, name="customer_eagleeye"),
    path("contracts", customer_contracts.home, name="customer_contracts"),
    path("flex", customer_flex.home, name="customer_flex"),
    path("bills", customer_bills.home, name="customer_bills"),
    path("profile", customer_profile.home, name="customer_profile"),
]
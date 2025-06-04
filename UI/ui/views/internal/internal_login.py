from ui.utils.security import login_view

def home(request):
    return login_view(request, '68533c84-5c4c-4c00-8b2f-aedf83e191f8', 'internal_dashboard', 'internal/internal_login.html')
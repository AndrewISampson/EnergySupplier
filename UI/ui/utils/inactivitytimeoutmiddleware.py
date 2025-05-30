import datetime
from django.conf import settings
from django.shortcuts import redirect

class InactivityTimeoutMiddleware:
    def __init__(self, get_response):
        self.get_response = get_response
        self.timeout = getattr(settings, "INACTIVITY_TIMEOUT", 900)  # 15 minutes default
        self.exempt_paths = [
            '/broker/login',
            '/customer/login',
            '/internal/login',
        ]

    def __call__(self, request):
        # Skip middleware on exempted paths
        if request.path == '/':
            return self.get_response(request)
        
        if any(request.path.startswith(p) for p in self.exempt_paths):
            return self.get_response(request)
        
        now = datetime.datetime.utcnow()
        path = request.path

        if request.session.get('last_activity'):
            last_activity = datetime.datetime.fromisoformat(request.session['last_activity'])
            elapsed = (now - last_activity).total_seconds()

            if elapsed > self.timeout:
                # Optional: log out user or clear session
                request.session.flush()
                request.session['inactivity_message'] = 'You have been logged out due to inactivity'

                if path.startswith('/broker/'):
                    return redirect('broker_login')
                elif path.startswith('/customer/'):
                    return redirect('customer_login')
                elif path.startswith('/internal/'):
                    return redirect('internal_login')
                else:
                    return redirect('master')

        # Update activity timestamp
        request.session['last_activity'] = now.isoformat()

        return self.get_response(request)

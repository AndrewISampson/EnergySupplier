from django import template

register = template.Library()

@register.simple_tag(takes_context=True)
def active_class(context, pattern):
    request = context.get('request')
    if request and request.path.startswith(pattern):
        return "active text-white bg-secondary rounded"
    return ""

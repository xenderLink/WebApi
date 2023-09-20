namespace Store.Extensions;

public sealed class ShortConstraint: IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out object? routeValue))
        {
            try
            {
                Convert.ToInt16(routeValue);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }
}
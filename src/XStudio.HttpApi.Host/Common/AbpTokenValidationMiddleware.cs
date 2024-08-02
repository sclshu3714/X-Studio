using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace XStudio.Common
{
    public class AbpTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public AbpTokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZDQkMtSFM1MTIiLCJraWQiOiI0RDRCRjI0RTAxQURGODY2OUExMDAzQkNEMDJDNzM5ODYyQ0NBRTk1IiwidHlwIjoib2lfYXVjK2p3dCIsImN0eSI6IkpXVCJ9.jerBTvd_QU6dludwCQ0GYfHsXcxCqcsyi3ttuTxIvhzHr1e1m2utDuHQKz6WNMmMzyueDWUllrHxup8OERUSJYRjM6YZ4WpG5bQyZD3TiM2CrTD7F9lEYetNeQqX4Os-GEm2B_lCAqpvS8gcJ1DNrpx-4DATgjXQcPhiXYkpPo415cFaSkq3n628B-fDRRzhQM7o_jNqoIViFtTJgKrgZEE3NnDDspf7FSpo4VUL5KU3tBF4Zxr2oXNjh9TLHXCU7tOxasVDlvtlDPe08OEVdcc9S660_itlyJF5Sr4zxznoT1GPUGlMDoxU0uRcthWohipMAJJ-Fn-IgUUrHdXKOg.AWI1TBJutmt4AlL3kKvcmw.8Tr0foOEVk4RSxUHWOnFN2vw6iU59ZZgOPu-2h9c5obKgJUVuv1rBVgFrqAC_VRko0YyKJhCBet1-acfLuKf55ANeWpFcnZhsETM9vQQZsBwg0mKgoFohN9Zb2o7EEWvPHCHBFez1M0h453q6rCmZBZQ4RVlQFXUS3VgjIdKpEvPHMM5vNUbzzKS0OAIZ9K5SKfYG1eczItu0vfUB4Q6t4YNzZ04T7-hvklSsCsDiU2jBQxljVbojCuJcPk1dUN63tZ29TpBRUqtpYIkGdE_Kcjn7xaMwksUnGh6ZajzUFH4Ixy_DLhEuSactA46nzYQk1C4meZsmrGfTmoZdvUOtcfgLSZwBBdfyiqZZrU24wTsAUcZ12uB6xh0bxByf9WCRps5Yy7iE_Y6-e122dxuK4eeTGxJbf6oNHqermFvEEHe3C4RYGb6oF3d_ysCUbrMUnuQHUeY6ZmVlwU8AplngdBDFJ_kwIT7yfv7iUpbgJyHnzLtvuIiVaK9i1vrWBlbBVZxWa2FMH6dp5SpFRMQcD1GjdZV66WT5YkHAR_MMKpjoZq146ozXr9Wwhh3ySMsLK1yRtiKANMLA9ub_GDHCS28547hCKiCnvykHEonFsY2_P8ZgxSV_UDZkUUKxzT3I0s5jgB__Vod_8t5d7CbzMJAYrd0Qj-699HdtjQz-NkNT6AeZBNYd0AWBoJnVWWe59YSk3w0WUmI2IimNktl0jT3B5vevrTZuqKfPSzspLDcJrhr_WSJ2aq-M-IP8EQKt4eUzIsTwQGwILUyV57Ky-GIwfrd6dT8vHHB-NYp6Vai9aUaIl4RjW0XqIAxv3pmfpBL5fWfkJpp9cvoLo9tvpIR0EV3BR6HVs1tqIhgPZSR9U8jLYYz1kX_A3IzHwVX_WXv5nFs3yu9_rBT8aAyHkbatsZSzRiKwN9zFBZIMNx9iCttmwgzEN7sbWKQf8nrUI2Ah5AmDy5ejs0GM2EgvWxljRx1tXxj5BDNtWJ1b3dyyFhKdZbO8F8oo8L7ev3s9BvMYSNIxdsGlSK0gOCvwt7r5udmYPF2a7p_kQIgx0DqjR173m3B704gKb6N0yxR3ed1JeMPapzW8bDIZZ9XdLma7Ka3GtkFXWB1LQF6YYioEikKWzb-Jx4QDAoifR03e0d1ua8SHChSs24RsF2YS5VtnRT0jfbnJ0z8jYiOt1kL22216tcPSz2Z-MwQWqGhVYxoA6U_dlxayo2zk3IkkGGPtnd5xALmA42i4YMHPu76reA36zyEhclFLI1s0W4UJgGF_fVuF9aJUkGIo4mEW4PjT6A3VfwwrutA1Ru8JGfoN7FMXDFy6NfR68JTrCYY75nCUbU5KldNQR266ttORfI1OhObo1e0UPVQrdGoGSSdnWfbrUCbdIwiYKU1d-MOJ-WcT_5hur0y9F-VsP9L5D9-9eRGgQEkp0CC3OAXIzLPlDGZuqnXhfAZ_g0MYPEGbDQsfl2ZJ1j5ynUkHkdj_yVxMxtGZqGvf-Q14TCTxqnv16H5qd95tLG971hkwxcy6ADFVFIa4V-R8IVjMYA71-fxezXbhVdRAnxVyRk1TkpIrOxHLAerBzywTrsvlYSPCCiXwr-ADTqbFBxhUvtFLd1JSupLN9ZqaonnfzltdfXY9iagZVwbvvzwjJiAP_JnaeTKAsr6Z_RaxBwvKREiXLBbJaA8NgW73OG_RzP5oBGx1eyH9GYGhz36QCWGPrJNKbwDcyO9hB8eytm57EKwseayXvszOMw8a2bdBmMmkXjiHgr5aE-1vkjnl-0E4hyuQeqODe2iHM02rHuZ8-MOL2xw-472bTgLbV-vzJMyGDbfmpU64vpbHJsA5SgeLBxPadf1cAzPs3AAf2f5DEuSXUnG9XBm2a0fbUw16quUAZDqSn3bfkuJd67DRf2YFCGTHBKhTThZ9V7N1EnRFQnfVvAbVH2gRhvo1wHWFVmFc4rAFpO1jYAVZQkQAaD9y4vjFp9HKDnBDAZeYipteUZ9zCeF9buGQ1B8ga9yTdP7ol1scXYFEcpWP8zE8zd6q11EVBclhSXtVWA3I2lhmMm4wFw44mEsx2KTIAPrfAP5_s8M7Y2L9EcP_VVd8jrBUlg3ki9gqKvh8ypFgrghrz5UdQ66cCd5PqHXo4AcroPAZfbvfAoAwHLgJuHt-Oswf1ISG3Anl1kqsqzpUXYLk9KpXYFx4ih6WibIlSgLT4rlyigqXlP2_U-mLTUJJ9SI5xrHvvxmXK1LyUsz6WbimHsAdz_pXMc4xjIhkN7ObxYJwGCpQJlB1CxZY1XMuHHCnD6zLj8niv0qGDM7sYXyin3icfkA6St8D9mCg5aFs0aJ8_OAc3ujHEjW2lYKsNRhkMo01-SKkYGiNQ4dQmPv9t2S4T0Dj3bUWGVYKplWWoUTNVE4X7TkPMCa5qeSjxZy0Vxk.dPe8xzXI0uNovJxoZM86Yx4BVLNTlTwoHTo1IPun45E";
                //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //return;
            }

            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // 在这里编写验证第三方Token的逻辑，可以调用第三方服务或验证Token有效性

            if (IsValidToken(token))
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "admin") }, "Bearer"));
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }

        private bool IsValidToken(string token)
        {
            // 在这里实现验证Token的逻辑，可以调用第三方服务或验证Token有效性
            // 返回true表示Token有效，false表示无效
            return true;
        }
    }
}

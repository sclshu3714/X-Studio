using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Themes;

namespace XStudio.Controllers.Themes {
    [Route("api/xstudio/[controller]/v{version:apiVersion}")]
    [ApiVersion(1.0)]
    [ApiController]
    public class ThemeController {
        private readonly IThemeService _themeService;

        public ThemeController(IThemeService themeService) {
            _themeService = themeService;
        }

        [HttpPost("changeTheme")]
        public async Task ChangeTheme(string newTheme) {
            await _themeService.SetThemeAsync(newTheme);
        }
        [HttpGet("getCurrentTheme")]
        public async Task<string?> GetCurrentTheme() {
            return await _themeService.GetThemeAsync();
        }
    }
}

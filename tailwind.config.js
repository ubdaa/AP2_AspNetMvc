/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml'
  ],
  theme: {
    fontSize: {
      sm: '0.750rem',
      base: '1rem',
      xl: '1.333rem',
      '2xl': '1.777rem',
      '3xl': '2.369rem',
      '4xl': '3.158rem',
      '5xl': '4.210rem',
    },
    fontFamily: {
      heading: 'Noto Sans Gothic',
      body: 'Noto Sans Gothic',
    },
    fontWeight: {
      normal: '400',
      bold: '700',
    },
    extend: {
      colors: {
        'text': '#01090c',
        'background': '#effcfe',
        'primary': '#16c0f3',
        'secondary': '#8592f9',
        'accent': '#6345f6',
      },
    },
  },
  plugins: [
    require('daisyui'),
  ],
  daisyui: {
    
    themes: ["light", "dark", "fantasy"],
  },
}


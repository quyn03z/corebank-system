/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        bank: {
          primary: '#1E3A8A',   // Xanh dương đậm chuẩn ngân hàng
          secondary: '#3B82F6', // Xanh điểm nhấn
          accent: '#10B981',    // Xanh lá báo biến động số dư dương
          danger: '#EF4444'     // Đỏ báo trừ tiền / lỗi
        }
      }
    },
  },
  plugins: [],
}
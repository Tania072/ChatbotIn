// Efecto al hacer scroll a la sección de información
window.addEventListener('scroll', () => {
  const info = document.querySelector('.info-section');
  const position = info.getBoundingClientRect().top;
  const screenPosition = window.innerHeight / 1.3;

  if (position < screenPosition) {
    info.classList.add('visible');
  }
});

// Transición suave cuando aparece
document.addEventListener('DOMContentLoaded', () => {
  const info = document.querySelector('.info-section');
  info.style.opacity = '0';
  info.style.transition = 'opacity 1s ease, transform 1s ease';
  info.style.transform = 'translateY(50px)';
});

document.querySelector('.btn').addEventListener('click', () => {
  // Efecto rápido al hacer clic
  const btn = document.querySelector('.btn');
  btn.style.transform = 'scale(0.95)';
  setTimeout(() => {
    btn.style.transform = 'scale(1)';
  }, 150);
});

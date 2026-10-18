window.gameInterop = (function () {
    const audioCtx = new (window.AudioContext || window.webkitAudioContext)();
    let sceneAnimId = null;
    let confettiAnimId = null;
    let sceneParticles = [];
    let confettiParticles = [];

    function playNote(freq) {
        const now = audioCtx.currentTime;
        const osc = audioCtx.createOscillator();
        const gain = audioCtx.createGain();
        osc.type = 'triangle';
        osc.frequency.setValueAtTime(freq, now);
        gain.gain.setValueAtTime(0, now);
        gain.gain.linearRampToValueAtTime(0.4, now + 0.01);
        gain.gain.exponentialRampToValueAtTime(0.0001, now + 2.5);
        osc.connect(gain);
        gain.connect(audioCtx.destination);
        osc.start(now);
        osc.stop(now + 2.5);
    }

    function playWinNotes(freqs) {
        freqs.forEach((f, i) => {
            setTimeout(() => playNote(f), i * 150);
        });
    }

    function resumeAudio() {
        if (audioCtx.state === 'suspended') {
            audioCtx.resume();
        }
    }

    function registerKeys(dotNetRef) {
        window.addEventListener('keydown', (e) => {
            const key = e.key.toLowerCase();
            if (key === '1' || key === '2') {
                e.preventDefault();
                dotNetRef.invokeMethodAsync('OnKeyPressed', key);
            }
        });
    }

    function initSceneEffect(effect) {
        cancelAnimationFrame(sceneAnimId);
        const canvas = document.getElementById('scene-overlay');
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
        sceneParticles = [];

        if (effect === 'none') return;

        for (let i = 0; i < 100; i++) {
            sceneParticles.push({
                x: Math.random() * canvas.width,
                y: Math.random() * canvas.height,
                size: Math.random() * 5 + 2,
                speed: Math.random() * 3 + 1,
                opacity: Math.random(),
                life: Math.random()
            });
        }

        function animate() {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            sceneParticles.forEach(p => {
                if (effect === 'rain') {
                    p.y += p.speed * 5;
                    ctx.strokeStyle = 'rgba(174,194,224,0.5)';
                    ctx.lineWidth = 1;
                    ctx.beginPath();
                    ctx.moveTo(p.x, p.y);
                    ctx.lineTo(p.x, p.y + 10);
                    ctx.stroke();
                } else if (effect === 'snow') {
                    p.y += p.speed;
                    p.x += Math.sin(p.y / 30);
                    ctx.fillStyle = 'rgba(255,255,255,0.8)';
                    ctx.beginPath();
                    ctx.arc(p.x, p.y, p.size / 2, 0, Math.PI * 2);
                    ctx.fill();
                } else if (effect === 'bubbles') {
                    p.y -= p.speed;
                    p.x += Math.sin(p.y / 50);
                    ctx.strokeStyle = 'rgba(255,255,255,0.4)';
                    ctx.beginPath();
                    ctx.arc(p.x, p.y, p.size * 2, 0, Math.PI * 2);
                    ctx.stroke();
                } else if (effect === 'hearts') {
                    p.y -= p.speed;
                    ctx.fillStyle = `rgba(255,100,100,${p.opacity})`;
                    ctx.font = `${p.size * 4}px serif`;
                    ctx.fillText('❤️', p.x, p.y);
                } else if (effect === 'stars') {
                    p.life += 0.02;
                    const alpha = Math.abs(Math.sin(p.life));
                    ctx.fillStyle = `rgba(255,255,200,${alpha})`;
                    ctx.beginPath();
                    ctx.arc(p.x, p.y, p.size / 2, 0, Math.PI * 2);
                    ctx.fill();
                }

                if (p.y > canvas.height) p.y = -20;
                if (p.y < -50) p.y = canvas.height + 20;
            });
            sceneAnimId = requestAnimationFrame(animate);
        }
        animate();
    }

    function startConfetti() {
        const canvas = document.getElementById('confetti');
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;

        confettiParticles = Array.from({ length: 150 }, () => ({
            x: Math.random() * canvas.width,
            y: Math.random() * canvas.height - canvas.height,
            size: Math.random() * 8 + 4,
            color: `hsl(${Math.random() * 360}, 80%, 60%)`,
            vel: Math.random() * 4 + 2,
            angle: Math.random() * 360
        }));

        function anim() {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            confettiParticles.forEach(p => {
                p.y += p.vel;
                p.angle += 2;
                ctx.fillStyle = p.color;
                ctx.save();
                ctx.translate(p.x, p.y);
                ctx.rotate(p.angle * Math.PI / 180);
                ctx.fillRect(-p.size / 2, -p.size / 2, p.size, p.size);
                ctx.restore();
                if (p.y > canvas.height) p.y = -20;
            });
            confettiAnimId = requestAnimationFrame(anim);
        }
        anim();
    }

    function stopConfetti() {
        cancelAnimationFrame(confettiAnimId);
        const canvas = document.getElementById('confetti');
        if (canvas) {
            const ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }
    }

    window.addEventListener('resize', () => {
        const scene = document.getElementById('scene-overlay');
        if (scene) {
            scene.width = window.innerWidth;
            scene.height = window.innerHeight;
        }
    });

    function enableMobileGuard() {
        document.addEventListener('touchmove', function (e) {
            if (e.target.closest && e.target.closest('.modal-content')) return;
            e.preventDefault();
        }, { passive: false });

        document.addEventListener('gesturestart', function (e) {
            e.preventDefault();
        });

        document.addEventListener('contextmenu', function (e) {
            e.preventDefault();
        });
    }

    return {
        playNote,
        playWinNotes,
        resumeAudio,
        registerKeys,
        initSceneEffect,
        startConfetti,
        stopConfetti,
        enableMobileGuard
    };
})();

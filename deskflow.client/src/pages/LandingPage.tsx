import '../styles/landing.css';

const CheckIcon = () => (
    <svg viewBox="0 0 12 12">
        <polyline points="2,6 5,9 10,3" />
    </svg>
);

const Star = () => <div className="star" />;

const Stars = () => (
    <div className="testimonial-stars">
        {[...Array(5)].map((_, i) => (
            <Star key={i} />
        ))}
    </div>
);

export function LandingPage() {
    return (
        <div className="landing">
            {/* ── Navigation ── */}
            <nav>
                <div className="nav-brand">
                    <div className="nav-logo">D</div>
                    <span className="nav-name">DeskFlow</span>
                </div>
                <div className="nav-links">
                    <a href="#">Features</a>
                    <a href="#">How it works</a>
                    <a href="#">Pricing</a>
                    <a href="#">Docs</a>
                </div>
                <a href="#" className="nav-cta">
                    Get started free
                </a>
            </nav>

            {/* ── Hero ── */}
            <section className="hero">
                <div className="container">
                    <div className="hero-badge">
                        <span className="hero-badge-dot" />
                        Multi-tenant · EN / AR · Built with .NET 10
                    </div>
                    <h1>
                        Support your customers
                        <br />
                        without the <span>chaos</span>
                    </h1>
                    <p>
                        DeskFlow gives your team a clean, fast helpdesk. Tickets, agents,
                        priorities, and replies — everything in one place.
                    </p>
                    <div className="hero-actions">
                        <a href="#" className="btn-primary">
                            <svg
                                width="16"
                                height="16"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                strokeWidth="2.5"
                                strokeLinecap="round"
                            >
                                <path d="M5 12h14M12 5l7 7-7 7" />
                            </svg>
                            Start for free
                        </a>
                        <a href="#" className="btn-secondary">
                            View demo
                        </a>
                    </div>
                    <p className="hero-note">
                        No credit card required · Free plan available · Setup in 2 minutes
                    </p>

                    {/* Hero Preview */}
                    <div className="hero-preview">
                        <div className="preview-bar">
                            <div className="preview-dot" style={{ background: '#ff5f57' }} />
                            <div className="preview-dot" style={{ background: '#febc2e' }} />
                            <div className="preview-dot" style={{ background: '#28c840' }} />
                            <div className="preview-url">app.deskflow.com/acme/dashboard</div>
                        </div>
                        <div className="preview-screen">
                            <div className="preview-topbar">
                                <span className="preview-title">Dashboard</span>
                                <span className="preview-btn">+ New Ticket</span>
                            </div>
                            <div className="preview-body">
                                <div className="preview-sidebar">
                                    <div className="preview-nav-item active">
                                        <span className="preview-nav-dot" />
                                        Dashboard
                                    </div>
                                    <div className="preview-nav-item">
                                        <span className="preview-nav-dot" />
                                        Tickets
                                    </div>
                                    <div className="preview-nav-item">
                                        <span className="preview-nav-dot" />
                                        Users
                                    </div>
                                    <div className="preview-nav-item">
                                        <span className="preview-nav-dot" />
                                        Audit Logs
                                    </div>
                                </div>
                                <div className="preview-content">
                                    <div className="preview-stats">
                                        <div className="preview-stat">
                                            <div className="preview-stat-n">24</div>
                                            <div className="preview-stat-l">Total</div>
                                        </div>
                                        <div className="preview-stat">
                                            <div
                                                className="preview-stat-n"
                                                style={{ color: '#2563eb' }}
                                            >
                                                12
                                            </div>
                                            <div className="preview-stat-l">Open</div>
                                        </div>
                                        <div className="preview-stat">
                                            <div
                                                className="preview-stat-n"
                                                style={{ color: '#d97706' }}
                                            >
                                                7
                                            </div>
                                            <div className="preview-stat-l">In Progress</div>
                                        </div>
                                        <div className="preview-stat">
                                            <div
                                                className="preview-stat-n"
                                                style={{ color: '#16a34a' }}
                                            >
                                                5
                                            </div>
                                            <div className="preview-stat-l">Resolved</div>
                                        </div>
                                    </div>
                                    <div className="preview-rows">
                                        <div className="preview-row">
                                            <span className="preview-ref">TKT-00001</span>
                                            <span className="preview-subject">
                                                Cannot login to account
                                            </span>
                                            <span className="pill pill-prog">In Progress</span>
                                            <span className="pill pill-hi">High</span>
                                        </div>
                                        <div className="preview-row">
                                            <span className="preview-ref">TKT-00002</span>
                                            <span className="preview-subject">
                                                Billing invoice incorrect
                                            </span>
                                            <span className="pill pill-open">Open</span>
                                            <span className="pill pill-hi">Critical</span>
                                        </div>
                                        <div className="preview-row">
                                            <span className="preview-ref">TKT-00003</span>
                                            <span className="preview-subject">
                                                Feature request: dark mode
                                            </span>
                                            <span className="pill pill-open">Open</span>
                                            <span className="pill pill-lo">Low</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* ── Stats ── */}
            <section
                className="section"
                style={{
                    background: '#f9fafb',
                    borderTop: '1px solid #f3f4f6',
                    borderBottom: '1px solid #f3f4f6',
                }}
            >
                <div className="container">
                    <div className="stats-row">
                        <div className="stat-item">
                            <div className="stat-number">12k+</div>
                            <div className="stat-label">Tickets resolved</div>
                        </div>
                        <div className="stat-item">
                            <div className="stat-number">98%</div>
                            <div className="stat-label">Satisfaction rate</div>
                        </div>
                        <div className="stat-item">
                            <div className="stat-number">3 min</div>
                            <div className="stat-label">Avg response time</div>
                        </div>
                        <div className="stat-item">
                            <div className="stat-number">2</div>
                            <div className="stat-label">Languages supported</div>
                        </div>
                    </div>
                </div>
            </section>

            {/* ── Features ── */}
            <section className="section">
                <div className="container">
                    <div className="section-label">Features</div>
                    <div className="section-title">
                        Everything your support team needs
                    </div>
                    <p className="section-sub">
                        From ticket creation to resolution — DeskFlow keeps your team
                        aligned and your customers happy.
                    </p>
                    <div className="features-grid">
                        <div className="feature-card">
                            <div className="feature-icon fi-indigo">
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="#4f46e5"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                >
                                    <rect x="2" y="3" width="20" height="14" rx="2" />
                                    <path d="M8 21h8M12 17v4" />
                                </svg>
                            </div>
                            <h3>Multi-tenant workspaces</h3>
                            <p>
                                Each company gets a completely isolated helpdesk. Data never
                                crosses tenant boundaries — enforced at the API layer.
                            </p>
                        </div>
                        <div className="feature-card">
                            <div className="feature-icon fi-teal">
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="#0d9488"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                >
                                    <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z" />
                                </svg>
                            </div>
                            <h3>Threaded conversations</h3>
                            <p>
                                Customers and agents stay in sync through a clean reply thread.
                                Every message is timestamped and attributed.
                            </p>
                        </div>
                        <div className="feature-card">
                            <div className="feature-icon fi-amber">
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="#d97706"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                >
                                    <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2" />
                                </svg>
                            </div>
                            <h3>Priority management</h3>
                            <p>
                                Triage tickets by Low, Medium, High, or Critical. Agents always
                                know what to tackle first.
                            </p>
                        </div>
                        <div className="feature-card">
                            <div className="feature-icon fi-blue">
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="#2563eb"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                >
                                    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
                                    <circle cx="9" cy="7" r="4" />
                                    <path d="M23 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
                                </svg>
                            </div>
                            <h3>Role-based access</h3>
                            <p>
                                Super Admins, Tenant Admins, and Agents each see exactly what
                                they need — nothing more.
                            </p>
                        </div>
                        <div className="feature-card">
                            <div className="feature-icon fi-green">
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="#16a34a"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                >
                                    <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
                                    <polyline points="14 2 14 8 20 8" />
                                    <line x1="16" y1="13" x2="8" y2="13" />
                                    <line x1="16" y1="17" x2="8" y2="17" />
                                </svg>
                            </div>
                            <h3>Full audit trail</h3>
                            <p>
                                Every action is logged — logins, ticket changes, invites, plan
                                changes. Complete visibility for compliance and trust.
                            </p>
                        </div>
                        <div className="feature-card">
                            <div className="feature-icon fi-purple">
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="#7c3aed"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                >
                                    <circle cx="12" cy="12" r="10" />
                                    <line x1="2" y1="12" x2="22" y2="12" />
                                    <path d="M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z" />
                                </svg>
                            </div>
                            <h3>English &amp; Arabic</h3>
                            <p>
                                Full EN/AR localization with automatic RTL layout switching.
                                Every label, message, and date adapts instantly.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            {/* ── How it works ── */}
            <section
                className="section"
                style={{
                    background: '#f9fafb',
                    borderTop: '1px solid #f3f4f6',
                    borderBottom: '1px solid #f3f4f6',
                }}
            >
                <div className="container">
                    <div className="section-label">How it works</div>
                    <div className="section-title">Up and running in minutes</div>
                    <div className="how-grid">
                        <div className="how-steps">
                            <div className="how-step">
                                <div className="how-step-num">1</div>
                                <div className="how-step-body">
                                    <h4>Create your workspace</h4>
                                    <p>
                                        Register your company, pick a subdomain, and invite your
                                        support agents. Takes about 2 minutes.
                                    </p>
                                </div>
                            </div>
                            <div className="how-step">
                                <div className="how-step-num">2</div>
                                <div className="how-step-body">
                                    <h4>Customers submit tickets</h4>
                                    <p>
                                        No account needed. Customers fill out a public form and get
                                        a reference number to track progress.
                                    </p>
                                </div>
                            </div>
                            <div className="how-step">
                                <div className="how-step-num">3</div>
                                <div className="how-step-body">
                                    <h4>Agents resolve and reply</h4>
                                    <p>
                                        Agents pick up tickets, reply in thread, update status and
                                        priority until the issue is closed.
                                    </p>
                                </div>
                            </div>
                            <div className="how-step">
                                <div className="how-step-num">4</div>
                                <div className="how-step-body">
                                    <h4>Track everything</h4>
                                    <p>
                                        Admins see full stats, team performance, and a complete
                                        audit log of every action taken.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div className="how-visual">
                            <div className="ticket-mock">
                                <div className="ticket-mock-header">
                                    <span className="ticket-mock-title">
                                        TKT-00001 — Cannot login to account
                                    </span>
                                    <span className="pill pill-prog">In Progress</span>
                                </div>
                                <div className="ticket-thread">
                                    <div className="msg">
                                        <div className="msg-avatar msg-av-c">J</div>
                                        <div className="msg-bubble msg-bubble-c">
                                            I keep getting &quot;Invalid credentials&quot; even though
                                            my password is correct.
                                        </div>
                                    </div>
                                    <div className="msg msg-right">
                                        <div className="msg-avatar msg-av-a">B</div>
                                        <div className="msg-bubble msg-bubble-a">
                                            Hi John, I can see your account. Let me reset your session
                                            — try again now.
                                        </div>
                                    </div>
                                    <div className="msg">
                                        <div className="msg-avatar msg-av-c">J</div>
                                        <div className="msg-bubble msg-bubble-c">
                                            Still getting the same error after the reset.
                                        </div>
                                    </div>
                                    <div className="msg msg-right">
                                        <div className="msg-avatar msg-av-a">B</div>
                                        <div className="msg-bubble msg-bubble-a">
                                            Escalating to the infra team now. You&apos;ll hear back
                                            within the hour.
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* ── Pricing ── */}
            <section className="section">
                <div className="container">
                    <div className="section-center">
                        <div className="section-label">Pricing</div>
                        <div className="section-title">Simple, honest pricing</div>
                        <p className="section-sub">
                            Start free, scale as your team grows. No hidden fees, no per-agent
                            tricks.
                        </p>
                    </div>
                    <div className="pricing-grid">
                        {/* Free */}
                        <div className="pricing-card">
                            <div className="pricing-plan">Free</div>
                            <div className="pricing-price">
                                \$0 <span>/ month</span>
                            </div>
                            <div className="pricing-desc">
                                Perfect for small teams getting started
                            </div>
                            <div className="pricing-features">
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Up to 5 team members
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Unlimited tickets
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    EN &amp; AR support
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Audit logs (30 days)
                                </div>
                            </div>
                            <a href="#" className="pricing-btn pricing-btn-outline">
                                Get started
                            </a>
                        </div>

                        {/* Pro */}
                        <div className="pricing-card featured">
                            <div className="popular-badge">Most popular</div>
                            <div className="pricing-plan">Pro</div>
                            <div className="pricing-price">
                                \$29 <span>/ month</span>
                            </div>
                            <div className="pricing-desc">For growing support teams</div>
                            <div className="pricing-features">
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Up to 50 team members
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Unlimited tickets
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Priority support
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Full audit history
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Advanced analytics
                                </div>
                            </div>
                            <a href="#" className="pricing-btn pricing-btn-filled">
                                Start free trial
                            </a>
                        </div>

                        {/* Enterprise */}
                        <div className="pricing-card">
                            <div className="pricing-plan">Enterprise</div>
                            <div className="pricing-price">
                                \$99 <span>/ month</span>
                            </div>
                            <div className="pricing-desc">For large organizations</div>
                            <div className="pricing-features">
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Unlimited members
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Dedicated support
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    Custom subdomain
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    SLA guarantees
                                </div>
                                <div className="pricing-feat">
                                    <div className="pricing-check">
                                        <CheckIcon />
                                    </div>
                                    SSO &amp; compliance
                                </div>
                            </div>
                            <a href="#" className="pricing-btn pricing-btn-outline">
                                Contact sales
                            </a>
                        </div>
                    </div>
                </div>
            </section>

            {/* ── Testimonials ── */}
            <section className="section testimonials">
                <div className="container">
                    <div className="section-center">
                        <div className="section-label">Testimonials</div>
                        <div className="section-title">Teams that switched love it</div>
                    </div>
                    <div className="testimonials-grid">
                        <div className="testimonial-card">
                            <Stars />
                            <p className="testimonial-text">
                                &quot;We moved our support team from a spreadsheet to DeskFlow
                                in one afternoon. The audit log alone saved us in a compliance
                                review.&quot;
                            </p>
                            <div className="testimonial-author">
                                <div
                                    className="testimonial-av"
                                    style={{ background: '#eef2ff', color: '#4f46e5' }}
                                >
                                    AJ
                                </div>
                                <div>
                                    <div className="testimonial-name">Alice Johnson</div>
                                    <div className="testimonial-role">
                                        Head of Support, Acme Corp
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="testimonial-card">
                            <Stars />
                            <p className="testimonial-text">
                                &quot;The Arabic interface was a game changer for our regional
                                team. Switching languages takes one click and everything —
                                including the layout — flips.&quot;
                            </p>
                            <div className="testimonial-author">
                                <div
                                    className="testimonial-av"
                                    style={{ background: '#f0fdfa', color: '#0d9488' }}
                                >
                                    DL
                                </div>
                                <div>
                                    <div className="testimonial-name">David Lee</div>
                                    <div className="testimonial-role">CTO, Beta LLC</div>
                                </div>
                            </div>
                        </div>
                        <div className="testimonial-card">
                            <Stars />
                            <p className="testimonial-text">
                                &quot;Clean UI, fast API, and the multi-tenancy model means we
                                can white-label it for every client. Exactly what we were
                                looking for.&quot;
                            </p>
                            <div className="testimonial-author">
                                <div
                                    className="testimonial-av"
                                    style={{ background: '#fffbeb', color: '#d97706' }}
                                >
                                    MK
                                </div>
                                <div>
                                    <div className="testimonial-name">Mohamed K.</div>
                                    <div className="testimonial-role">
                                        Product Manager, TechCo
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* ── CTA ── */}
            <section style={{ padding: '80px 0 0' }}>
                <div className="cta-section">
                    <h2>Ready to simplify support?</h2>
                    <p>
                        Set up your helpdesk in minutes. Free plan, no credit card, no
                        friction.
                    </p>
                    <a href="#" className="cta-btn">
                        Create your workspace
                    </a>
                </div>
            </section>

            {/* ── Footer ── */}
            <footer>
                <div className="container">
                    <div className="footer-inner">
                        <div className="nav-brand">
                            <div className="nav-logo">D</div>
                            <span className="nav-name">DeskFlow</span>
                        </div>
                        <div className="footer-links">
                            <a href="#">Features</a>
                            <a href="#">Pricing</a>
                            <a href="#">Docs</a>
                            <a href="#">GitHub</a>
                            <a href="#">Privacy</a>
                        </div>
                        <span className="footer-copy">
                            © 2026 DeskFlow. Built with .NET 10 + React 18.
                        </span>
                    </div>
                </div>
            </footer>
        </div>
    );
}
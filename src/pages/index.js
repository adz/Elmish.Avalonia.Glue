import React from 'react';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import styles from './index.module.css';

export default function Home() {
  return (
    <Layout
      title="Elmish.Avalonia.Glue"
      description="Avalonia-first Elmish integration that keeps AXAML, bindings, preview, and DevTools normal."
    >
      <header className={styles.hero}>
        <div className={styles.heroInner}>
          <div className={styles.kicker}>Avalonia-first Elmish glue</div>
          <h1>Normal Avalonia views, F#-shaped state.</h1>
          <p className={styles.lead}>
            Avalonia has AXAML, bindings, design-time preview, and DevTools. Elmish has immutable state, explicit
            messages, and one update loop. Elmish.Avalonia.Glue explores how to bridge them while keeping the Avalonia
            workflow intact.
          </p>
          <p className={styles.lead}>
            Start with <code>Projection</code> for explicit CLR-facing viewmodels, or <code>ElmView</code> for immutable
            F# view records and a thin bindable host.
          </p>
          <div className={styles.actions}>
            <Link className="button button--primary" to="/docs/guides/start">
              Start here
            </Link>
          </div>
        </div>
      </header>
    </Layout>
  );
}

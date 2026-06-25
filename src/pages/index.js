import React from 'react';
import clsx from 'clsx';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import styles from './index.module.css';

const cards = [
  {
    title: 'Start',
    body: 'Read the short path that explains what this repo is, where the two authoring families differ, and how to preview them.',
    to: '/docs/guides/start',
  },
  {
    title: 'API',
    body: 'Jump into curated landing pages for Core, Avalonia, Projection, and ElmView instead of a namespace dump.',
    to: '/docs/api',
  },
  {
    title: 'Examples',
    body: 'Open executable examples that show observed output alongside focused source snippets.',
    to: '/docs/examples',
  },
  {
    title: 'Integrations',
    body: 'See how the repository fits Avalonia and Elmish without changing the normal tooling story.',
    to: '/docs/integrations',
  },
];

function FeatureCard({ title, body, to }) {
  return (
    <Link className={styles.card} to={to}>
      <h3>{title}</h3>
      <p>{body}</p>
      <span className={styles.cardCta}>Read more</span>
    </Link>
  );
}

export default function Home() {
  return (
    <Layout
      title="Elmish.Avalonia.Glue"
      description="Avalonia-first Elmish integration with curated docs, examples, and versioned API hubs."
    >
      <header className={styles.hero}>
        <div className={styles.heroInner}>
          <div className={styles.kicker}>Avalonia-first, F#-shaped UI authoring</div>
          <h1>Normal Avalonia tooling. Smaller authored UI surfaces.</h1>
          <p className={styles.lead}>
            This repository compares two authoring families on top of Elmish: explicit Projection viewmodels and the
            F#-first ElmView path. The docs are organized around what readers need to do, not around namespaces.
          </p>
          <div className={styles.actions}>
            <Link className={clsx('button button--primary', styles.primaryButton)} to="/docs/guides/start">
              Start here
            </Link>
            <Link className="button button--secondary button--outline" to="/docs/api">
              Browse the API hubs
            </Link>
          </div>
        </div>
      </header>

      <main className={styles.main}>
        <section className={styles.grid}>
          {cards.map((card) => (
            <FeatureCard key={card.title} {...card} />
          ))}
        </section>

        <section className={styles.panel}>
          <h2>What this site emphasizes</h2>
          <ul>
            <li>Curated landing pages for each major package and family.</li>
            <li>Examples that run during docs generation and publish their observed output.</li>
            <li>Versioned docs snapshots so the current state and archived state can coexist.</li>
            <li>Direct source links back to the repository files that implement the documented behavior.</li>
          </ul>
        </section>
      </main>
    </Layout>
  );
}
